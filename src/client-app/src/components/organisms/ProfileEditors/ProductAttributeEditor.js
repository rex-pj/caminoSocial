import React, { Fragment } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import AsyncSelect from "react-select/async";
import {
  ButtonOutlinePrimary,
  ButtonOutlineDanger,
} from "../../atoms/Buttons/OutlineButtons";
import styled from "styled-components";
import ProductAttributeValueEditor from "./ProductAttributeValueEditor";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${PrimaryTextbox} {
    max-width: 100%;
    width: 100%;
  }

  .cate-selection {
    > div {
      border: 1px solid ${(p) => p.theme.color.primaryDivide};
    }
  }

  ${AsyncSelect} {
    max-width: 100%;
  }
`;

export default (props) => {
  const { attribute } = props;
  const { attributeValues } = attribute;

  const onAddAttributeValue = () => {
    let { attribute } = props;
    let { attributeValues } = attribute;

    if (!attributeValues) {
      attributeValues = [];
    }

    attributeValues.push({
      label: "",
      priceAdjustment: 0,
      quantity: 0,
      displayOrder: 0,
    });

    attribute.attributeValues = attributeValues;
    props.onAttributeChange(attribute);
  };

  const onRemoveAttribute = (currentAttr) => {
    props.onRemoveAttribute(currentAttr);
  };

  const handleInputChange = (evt) => {
    let { attribute } = props;
    const { name, value } = evt.target;
    attribute[name] = value;
    props.onAttributeChange(attribute);
  };

  const onRemoveAttributeValue = (currentAttributeValue) => {
    let { attribute } = props;
    let { attributeValues } = attribute;
    const index = attributeValues.indexOf(currentAttributeValue);
    attributeValues.splice(index, 1);

    attribute.attributeValues = attributeValues;
    props.onAttributeChange(attribute);
  };

  const onAttributeValueChange = (e, index) => {
    let { attribute } = props;
    let { attributeValues } = attribute;
    attributeValues[index] = e;

    attribute.attributeValues = attributeValues;
    props.onAttributeChange(attribute);
  };

  return (
    <Fragment>
      <FormRow>
        <div className="row mb-2">
          <div className="col-auto">
            <ButtonOutlinePrimary
              type="button"
              size="xs"
              title="Add attribute value"
              onClick={onAddAttributeValue}
            >
              <FontAwesomeIcon icon="plus" />
            </ButtonOutlinePrimary>
          </div>
          <div className="col-6 col-xl-4">
            <AsyncSelect
              className="cate-selection"
              placeholder="Select attribute"
            />
          </div>
          <div className="col-6 col-xl-4">
            <AsyncSelect
              className="cate-selection"
              placeholder="Select control type"
            />
          </div>
          <div className="col-6 col-xl-2">
            <PrimaryTextbox
              name="displayOrder"
              value={attribute.displayOrder ? attribute.displayOrder : ""}
              placeholder="Display Order"
              onChange={handleInputChange}
            />
          </div>
          <div className="col-auto">
            <ButtonOutlineDanger
              type="button"
              size="xs"
              title="Remove this attribute"
              onClick={() => onRemoveAttribute(attribute)}
            >
              <FontAwesomeIcon icon="times" />
            </ButtonOutlineDanger>
          </div>
        </div>
        {attributeValues
          ? attributeValues.map((attrVal, index) => {
              return (
                <ProductAttributeValueEditor
                  key={index}
                  attributeValue={attrVal}
                  onRemoveAttributeValue={onRemoveAttributeValue}
                  onAttributeValueChange={(e) =>
                    onAttributeValueChange(e, index)
                  }
                ></ProductAttributeValueEditor>
              );
            })
          : null}
      </FormRow>
    </Fragment>
  );
};
