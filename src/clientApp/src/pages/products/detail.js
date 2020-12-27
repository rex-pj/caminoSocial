import React, { useEffect } from "react";
import { UrlConstant } from "../../utils/Constants";
import Detail from "../../components/templates/Product/Detail";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import { useQuery } from "@apollo/client";
import styled from "styled-components";
import {
  productQueries,
  userQueries,
  farmQueries,
} from "../../graphql/fetching/queries";
import { withRouter } from "react-router-dom";
import ProductItem from "../../components/organisms/Product/ProductItem";
import { TertiaryHeading } from "../../components/atoms/Heading";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";
import DetailLayout from "../../components/templates/Layout/DetailLayout";

const RelationBox = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { id } = params;
  const [state] = useStore(false);

  const { loading, data, error, refetch } = useQuery(
    productQueries.GET_PRODUCT,
    {
      variables: {
        criterias: {
          id: parseFloat(id),
        },
      },
    }
  );

  const userIdentityId = data?.product?.createdByIdentityId;
  const { data: authorData } = useQuery(userQueries.GET_USER_INFO, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userId: userIdentityId,
      },
    },
  });

  const { data: userFarmData } = useQuery(farmQueries.GET_USER_FARMS_TITLE, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userIdentityId: userIdentityId,
        page: 1,
        pageSize: 3,
      },
    },
  });

  const {
    relevantLoading,
    data: relevantData,
    error: relevantError,
  } = useQuery(productQueries.GET_RELEVANT_PRODUCTS, {
    variables: {
      criterias: {
        id: parseFloat(id),
        page: 1,
        pageSize: 8,
      },
    },
  });

  useEffect(() => {
    if (state.type === "PRODUCT" && state.id) {
      refetch();
    }
  }, [state, refetch]);

  if (loading || !data) {
    return <Loading>Loading...</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { product: productData } = data;
  let product = { ...productData };

  const breadcrumbs = [
    {
      title: "Products",
      url: "/products/",
    },
    {
      isActived: true,
      title: product.name,
    },
  ];

  if (product.thumbnails && product.thumbnails.length > 0) {
    product.images = product.thumbnails.map((item) => {
      let image = { ...item };

      if (image.pictureId > 0) {
        image.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${image.pictureId}`;
        image.url = `${process.env.REACT_APP_CDN_PHOTO_URL}${image.pictureId}`;
      }
      return image;
    });
  }

  if (product.productFarms) {
    product.productFarms = product.productFarms.map((pf) => {
      let productFarm = { ...pf };
      productFarm.url = `/farms/${pf.farmId}`;
      return productFarm;
    });
  }

  const renderRelevants = (relevantLoading, relevantData, relevantError) => {
    if (relevantLoading || !relevantData) {
      return <Loading className="mt-3">Loading...</Loading>;
    } else if (relevantError) {
      return <ErrorBlock>Error!</ErrorBlock>;
    }
    const { relevantProducts } = relevantData;
    const relevants = relevantProducts.map((item) => {
      let productItem = { ...item };
      productItem.url = `${UrlConstant.Product.url}${productItem.id}`;
      if (productItem.thumbnails) {
        const thumbnail = productItem.thumbnails[0];
        if (thumbnail && thumbnail.pictureId > 0) {
          productItem.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${thumbnail.pictureId}`;
        }
      }

      productItem.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoCode) {
        productItem.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
      }

      if (productItem.productFarms) {
        productItem.productFarms = productItem.productFarms.map((pf) => {
          let productFarm = { ...pf };
          productFarm.url = `/farms/${pf.farmId}`;
          return productFarm;
        });
      }

      return productItem;
    });

    return (
      <RelationBox className="mt-3">
        <TertiaryHeading>Other Products</TertiaryHeading>
        <div className="row">
          {relevants
            ? relevants.map((item, index) => {
                return (
                  <div
                    key={index}
                    className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                  >
                    <ProductItem product={item} />
                  </div>
                );
              })
            : null}
        </div>
      </RelationBox>
    );
  };

  const getAuthorInfo = () => {
    if (!authorData) {
      return {};
    }
    const { userInfo } = authorData;
    const authorInfo = { ...userInfo };
    if (authorData) {
      const { userPhotos } = authorData;
      const avatar = userPhotos.find((item) => item.photoType === "AVATAR");
      if (avatar) {
        authorInfo.userAvatar = avatar;
      }
      const cover = userPhotos.find((item) => item.photoType === "COVER");
      if (cover) {
        authorInfo.userCover = cover;
      }
    }

    if (userFarmData) {
      const { userFarms } = userFarmData;
      const { collections } = userFarms;
      authorInfo.farms = collections;
    }
    return authorInfo;
  };

  return (
    <DetailLayout author={getAuthorInfo()}>
      <Breadcrumb list={breadcrumbs} />
      <Detail product={product} breadcrumbs={breadcrumbs} />
      {renderRelevants(relevantLoading, relevantData, relevantError)}
    </DetailLayout>
  );
});
