import React, { Fragment } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import AsyncSelect from "react-select/async";
import { ButtonOutlinePrimary } from "../../atoms/Buttons/OutlineButtons";
import styled from "styled-components";
import ProductAttributeValueEditor from "./ProductAttributeValueEditor";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${PrimaryTextbox} {
    max-width: 100%;
    width: 100%;
  }

  .cate-selection {
    z-index: 10;

    > div {
      border: 1px solid ${(p) => p.theme.color.primaryDivide};
    }
  }

  ${AsyncSelect} {
    max-width: 100%;
  }
`;

export default () => {
  return (
    <Fragment>
      <FormRow>
        <div className="row mb-2">
          <div className="col-5 col-xl-4">
            <AsyncSelect
              className="cate-selection"
              placeholder="Select attribute"
            />
          </div>
          <div className="col-5 col-xl-4">
            <AsyncSelect
              className="cate-selection"
              placeholder="Select control type"
            />
          </div>
          <div className="col-2 col-xl-2">
            <ButtonOutlinePrimary
              type="button"
              size="xs"
              title="Add attribute value"
            >
              <FontAwesomeIcon icon="plus"></FontAwesomeIcon>
            </ButtonOutlinePrimary>
          </div>
        </div>
        <ProductAttributeValueEditor />
      </FormRow>
    </Fragment>
  );
};
